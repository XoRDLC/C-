using XX = Methods.Override;


namespace Methods{
	class Override{
		private string value;
		private int price;
		
		public void Test(){}
		public void Test(ref int x){}
		public void Test(int x){}
		public void Test(out int x, ref int y){x = 2;}
		public Override(string value) => this.value = value;
		public Override(string value, int price): this(value){			
			this.price = price;			
		}
		public void PrintName() => System.Console.WriteLine(value);
		public void SetValue(string value) => this.value = value;
		public void PrintAll() { 
			System.Console.Write(this.value);
			System.Console.Write("\t");
			System.Console.WriteLine(this.price);}
	}
}

class Test{
	public static void Main(){
		XX a = new XX("Hi");
		int y = 4;
		a.Test(out int x, ref y);
		System.Console.WriteLine(x);
		a.PrintName();
		a.SetValue("hohoho");
		a.PrintName();
		XX b = new XX("Priced", 1000);
		b.PrintAll();
	}
}