using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//https://social.msdn.microsoft.com/Forums/en-US/b5721e4f-6b58-465e-82eb-629613c7de4a/get-url-from-browser-in-c?forum=netfxnetcom
namespace Library.Net.DataStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INTERNET_CACHE_ENTRY_INFO
    {
        public UInt32 dwStructSize;
        public string lpszSourceUrlName;
        public string lpszLocalFileName;
        public UInt32 CacheEntryType;
        public UInt32 dwUseCount;
        public UInt32 dwHitRate;
        public UInt32 dwSizeLow;
        public UInt32 dwSizeHigh;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ExpireTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;
        public IntPtr lpHeaderInfo;
        public UInt32 dwHeaderInfoSize;
        public string lpszFileExtension;
        public UInt32 dwExemptDelta;
    };
}


namespace Library.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using DataStructs;

    public sealed class InternetCache
    {
        private const int ERROR_INVALID_PARAMETER = 87;
        private const int ERROR_INSUFFICIENT_BUFFER = 122;
        private const int ERROR_NO_MORE_ITEMS = 259;
        private const int ERROR_NO_TOKEN = 1008;

        /// <summary>
        /// UrlCache functionality is taken from:
        /// Scott McMaster (EMAIL REMOVED)
        /// CodeProject article
        /// 
        /// There were some issues with preparing URLs
        /// for RegExp to work properly. This is
        /// demonstrated in AllForms.SetupCookieCachePattern method
        /// 
        /// urlPattern:
        /// . Dump the entire contents of the cache.
        /// Cookie: Lists all cookies on the system.
        /// Visited: Lists all of the history items.
        /// Cookie:.*\.example\.com Lists cookies from the example.com domain.
        /// http://www.example.com/example.html$: Lists the specific named file if present
        /// \.example\.com: Lists any and all entries from *.example.com.
        /// \.example\.com.*\.gif$: Lists the .gif files from *.example.com.
        /// \.js$: Lists the .js files in the cache.
        /// </summary>
        /// <param name="urlPattern"></param>
        /// <returns></returns>
        public static IEnumerable<INTERNET_CACHE_ENTRY_INFO> FindUrlCacheEntries(string urlPattern)
        {
            var results = new List<INTERNET_CACHE_ENTRY_INFO>();

            var buffer = IntPtr.Zero;
            UInt32 structSize;

            //This call will fail but returns the size required in structSize
            //to allocate necessary buffer
            var hEnum = FindFirstUrlCacheEntry(null, buffer, out structSize);
            try
            {
                if (hEnum == IntPtr.Zero)
                {
                    var lastError = Marshal.GetLastWin32Error();
                    switch (lastError)
                    {
                        case ERROR_INSUFFICIENT_BUFFER:
                            buffer = Marshal.AllocHGlobal((int)structSize);
                            hEnum = FindFirstUrlCacheEntry(urlPattern, buffer, out structSize);
                            break;
                        case ERROR_NO_TOKEN:
                        case ERROR_NO_MORE_ITEMS:
                            return results.AsEnumerable();
                    }
                }

                var result = (INTERNET_CACHE_ENTRY_INFO)Marshal.PtrToStructure(buffer, typeof(INTERNET_CACHE_ENTRY_INFO));
                try
                {
                    if (Regex.IsMatch(result.lpszSourceUrlName, urlPattern, RegexOptions.IgnoreCase))
                        results.Add(result);
                }
                catch (ArgumentException ae)
                {
                    throw new ApplicationException("Invalid regular expression, details=" + ae.Message);
                }

                if (buffer != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                    catch
                    {
                    }
                    buffer = IntPtr.Zero;
                }

                while (true)
                {
                    var nextResult = FindNextUrlCacheEntry(hEnum, buffer, out structSize);
                    structSize *= 4;
                    if (nextResult != 1) //TRUE
                    {
                        var lastError = Marshal.GetLastWin32Error();
                        switch (lastError)
                        {
                            case ERROR_INSUFFICIENT_BUFFER:
                                buffer = Marshal.AllocHGlobal((int)structSize);
                                FindNextUrlCacheEntry(hEnum, buffer, out structSize);
                                break;
                            case ERROR_NO_MORE_ITEMS:
                            case ERROR_INVALID_PARAMETER:
                                return results.AsEnumerable();
                        }
                    }

                    if (buffer != IntPtr.Zero)
                    {
                        result = (INTERNET_CACHE_ENTRY_INFO)Marshal.PtrToStructure(buffer, typeof(INTERNET_CACHE_ENTRY_INFO));
                        if (Regex.IsMatch(result.lpszSourceUrlName, urlPattern, RegexOptions.IgnoreCase))
                            results.Add(result);


                        try
                        {
                            Marshal.FreeHGlobal(buffer);
                        }
                        catch
                        {
                        }
                        buffer = IntPtr.Zero;
                    }
                }
            }
            finally
            {
                if (hEnum != IntPtr.Zero)
                    FindCloseUrlCache(hEnum);
                if (buffer != IntPtr.Zero)
                    try
                    {
                        Marshal.FreeHGlobal(buffer);
                    }
                    catch
                    {
                    }
            }
        }

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern IntPtr FindFirstUrlCacheEntry(string lpszUrlSearchPattern,
                                                           IntPtr lpFirstCacheEntryInfo,
                                                           out UInt32 lpdwFirstCacheEntryInfoBufferSize);
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern long FindNextUrlCacheEntry(IntPtr hEnumHandle,
                                                        IntPtr lpNextCacheEntryInfo,
                                                        out UInt32 lpdwNextCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern long FindCloseUrlCache(IntPtr hEnumHandle);

    }
}

