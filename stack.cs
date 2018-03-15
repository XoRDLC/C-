using System.Text;
using static System.Console;

class F{}

class S : F {}

class T : F {}

class Stack{
	private int pos;
	object[] data = new object[5];
	public object pp{ //push-pop
		get{ return data[--pos]; }
		set{ data[pos++] = value;}
	}
	public int Length() => data.Length;
	public int Elements() => pos+1;
}

class Start {
	static void Main(){
		Stack stack = new Stack();
		
		stack.pp = new F();
		stack.pp = new S();
		stack.pp = new Start();
		stack.pp = new StringBuilder();
		stack.pp = new S();
		
		WriteLine(stack.Elements());
		for( int i = 0; i<stack.Elements(); i++){
			object o = stack.pp;
			WriteLine(o.GetType());
			if( o is Start start)
				WriteLine($"----{start.val()}");			
		}
	}
	private string val() => "hi hi hi";
}