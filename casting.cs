using System;

class First{
	public string name = "-";
	public long Price{get; set;}
	private string Name{get;}
	public void prn(First first){
		Console.WriteLine(first.ToString());
	}
	public virtual string prn() => name + this.ToString();
}

sealed class Second: First{
	new public string name = "-"; //hiding inhereted member (keyw 'new') /or 'public new'
	public string Name{get => name; set => this.name = value;} //to hide private members no need 'new'
	public decimal Weight{get; set;}
	public override string prn() => $"new impl:{name + base.prn()}";
}

// class Thirds : Second {} // – Second is sealed, can't be inhereted

class J{
	static void Main()
	{
		First f = new First();
		First fs = new Second();
		f.Price = 1000;
		fs.Price = 3;
		Second s = new Second();
		s.Price = 14;
		s.Weight = 22;
		First us = s;
		Second s2 = new Second();
		if (us is Second supercast && (supercast.Weight = s.Weight + 1M) == 23){
			Console.WriteLine($"supercast = {supercast.Weight}");
			Console.WriteLine($"s = {s.Weight}");
			Console.WriteLine($"s = supercast: {s == supercast}");}
		else
			Console.WriteLine($"or {nameof(us)} not Second, or supercast weight <> {s.Weight}");
		
	Console.WriteLine($"{(string)us.ToString()==nameof(Second)}\t{(string)us.ToString()}\t{nameof(Second)}");
		
		Console.WriteLine($"{f is First}\t{f.prn()}");
		Console.WriteLine($"{fs is Second}\t{fs.prn()}");
		Console.WriteLine($"{s is Second}\t{s.prn()}");
		Console.WriteLine($"{(Second)us is Second}\t{us.prn()}");
		Console.WriteLine($"{s2 is Second}\t{s2.prn()}");
		Console.WriteLine($"{f is Second}\t{f.prn()}");	
		Console.WriteLine($"{(f as Second)?.Weight}"); // null: (f as Second).Weight -> NullReferenceException
		try{
			//f = null;
			Console.WriteLine($"{(f as Second).Weight}");			
		}
		catch(NullReferenceException e){
			if (f is null) 
				Console.WriteLine($"variable f is null\n{e}");
			else 
				Console.WriteLine($"casting error - (f as Second).Weight\n{e}");
		}
		
		try{
			Console.WriteLine($"{((Second)f).Weight}"); // (Second)f -> InvalidCastException
		}
		catch(InvalidCastException e){
			Console.WriteLine($"casting error - (Second)f\n{e}");
		}
	}
}