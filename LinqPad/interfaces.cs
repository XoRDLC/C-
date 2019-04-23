using System;

interface I1 { void Foo(); }
interface I2 { int Foo(); }

class StrictParent: 		I1{ void I1.Foo() 		=> Console.WriteLine("StrP: "  + this.GetType().Name); }
class SoftParent  : 		I1{ public void Foo() 	=> Console.WriteLine("SftP: "  + this.GetType().Name); }
class Child1: StrictParent, I1{ public void Foo() 	=> Console.WriteLine("Chld1: " + this.GetType().Name); }
class Child2: SoftParent,   I1{	public new void Foo()=>Console.WriteLine("Chld2: " + this.GetType().Name); }
class Child3: StrictParent    {	public void Foo()	=> Console.WriteLine("Chld3: " + this.GetType().Name); }

class I : I1, I2 {
	public void Foo() => Console.WriteLine("I1");
	int I2.Foo() { Console.WriteLine("I2"); return 42; }
}

class GreatI : I2 {
	int I2.Foo() 			  { Console.WriteLine("calling Foo"); return Foo();}
	public virtual int Foo()  { Console.WriteLine("GreatI: " + this.GetType().Name); return 42; }
}

class ChildGreat : GreatI {
	public override int Foo() { Console.WriteLine("ChildG: " + this.GetType().Name); return 42; }
}

class M{
	static void Main() {
		Use_I_Interface();
		Use_Child1();
		Use_Child2();
		Use_Child3();
		Use_GreatI();
		Use_ChildGreat();		
	}

	static void Use_ChildGreat() {
		GreatI gi = new GreatI();
		gi.Foo();
		((I2)gi).Foo();
		Console.WriteLine();	
	}

	static void Use_GreatI() {
		ChildGreat gi = new ChildGreat();
		gi.Foo();
		((I2)gi).Foo();
		((GreatI)gi).Foo();
		Console.WriteLine();	
	}

	static void Use_I_Interface() {
		I i = new I();
		i.Foo();
		((I1)new I()).Foo();
		((I2)new I()).Foo();
		Console.WriteLine();
	}

	static void Use_Child1() {
		Child1 ch1 = new Child1();
		ch1.Foo();
		((I1)ch1).Foo();
		Console.WriteLine();
	}

	static void Use_Child2() {
		Child2 ch2 = new Child2();
		ch2.Foo();
		((I1)ch2).Foo();	
		((SoftParent)ch2).Foo();
		Console.WriteLine();	
	}

	static void Use_Child3() {
		Child3 ch3 = new Child3();
		ch3.Foo();
		((I1)ch3).Foo();
		Console.WriteLine();	
	}
}