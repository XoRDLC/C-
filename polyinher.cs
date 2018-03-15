using System;

class First{
	private string name;
	public First(){
		name = this.ToString();
	}
	public long Price{get; set;}
	public void prn(First f){
		Console.WriteLine(f.name); //polymorphism
	}
}

class Second: First{
	public decimal Weight{get; set;}
}

class MainClass{
	static void Main()
	{
		First a = new Second(){}; //polymorphism
		a.Price = 3;
		Second snd = new Second(){};
		snd.Price = 2;
		snd.Weight = 4;
		
		Console.WriteLine($"{a.Price}\t{null}");
		Console.WriteLine($"{snd.Price}\t{snd.Weight}");
		a.prn(a);		//polymorphism
		snd.prn(a);		//polymorphism
		a.prn(snd);		//polymorphism
		snd.prn(snd);	//polymorphism
		a.prn(new First());//polymorphism
		new Second().prn(new First());//polymorphism
	}
}