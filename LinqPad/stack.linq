<Query Kind="Program">
  <Namespace>static System.Console</Namespace>
</Query>

abstract class F{}
class S : F {}
class T : F {}

class Stack{
	private int pos;
	object[] data = new object[5];
	//IndexOutOfRangeException
	public object pp{  //pop-push property
		get{ try{return data[--pos];} catch(IndexOutOfRangeException){Console.WriteLine($"stack empty");} return null; }  
		set{ try{data[pos++] = value;}catch(IndexOutOfRangeException){--pos;Console.WriteLine($"stack limit reached. Can't push {pos+1}-element, type of {value.GetType().Name}");}}	
	}
	//IndexOutOfRangeException
	public object this[int i]{  //indexer
		get{ try{return data[i];}catch(IndexOutOfRangeException e){Console.WriteLine($"out of stack range\n{e}");} return null; }	
		set{ try{data[i] = value;}catch(IndexOutOfRangeException e){Console.WriteLine($"out of stack range\n{e}");}}	
	}
	public int Length() => data.Length;
	public int Elements() => pos;
}

void Main(){
	Stack stack = new Stack();
	
	stack.pp = 2M;
	stack.pp = new S();
	stack.pp = new T();
	stack.pp = new StringBuilder();
	stack.pp = new int[4];
	stack.pp = 22.2;
	stack.pp = 123;
	stack.pp = new Object();
			
	Console.WriteLine($"---{stack.Elements()}---");
	Console.WriteLine(object.Equals(stack.pp, stack.pp));
	Console.WriteLine($"---{stack.Elements()}---");	
	
	for( int i = 0; i<stack.Length(); i++){ //IndexOutOfRangeException
		WriteLine((stack.pp)?.GetType().Name);
	}
	
	
}