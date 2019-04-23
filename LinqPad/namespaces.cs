extern alias G;

namespace N{
	class A{
		public class B{
			public B(){System.Console.WriteLine("Inner B");}}
		static void Main(){
			new A.B();
			new global::A.B();
			new G::A.B();
			new G.A.B();
		}		
	}	
}

namespace A{
	public class B{
		public B(){
			System.Console.WriteLine("Outer B");
		}
	}
}

namespace G{
	namespace A{
		public class B{
			public B(){
				System.Console.WriteLine("Inner G");
			}
		}
	}
}
