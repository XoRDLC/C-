using Prop;

namespace Prop{
	public class Par{
		private decimal price;
		
		public decimal Price{
			get{return price;}
			set{this.price = value;} 
		}	
		public decimal Weight{get; set;} = 1;	
		public decimal Value{get; internal set;} = 5;
	}
}

class Test{
	static void Main(){
		Par price = new Par();
		price.Price = 20M;
		price.Weight = 1.5M;
		price.Value = 6M;
		System.Console.WriteLine($"{price.Price}\t{price.Weight}\t{price.Value}");
	}
}