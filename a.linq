<Query Kind="Program">
  <Namespace>static System.Console</Namespace>
</Query>

class F{}

class S : F {}

class T : F {}

class Stack{
	private int pos;
	object[] data = new object[5];
	public object pp{
		get{ return data[--pos]; }
		set{ data[pos++] = value;}
	}
	public int Length() => data.Length;
	public int Elements() => pos;
}

void Main(){
	Stack stack = new Stack();
	
	stack.pp = new F();
	stack.pp = new S();
	stack.pp = new T();
	stack.pp = new StringBuilder();
	stack.pp = new int[4];
	
	Console.WriteLine(stack.Elements());
	for( int i = 0; i<stack.Length(); i++){
		WriteLine((stack.pp).GetType());
	}
}
