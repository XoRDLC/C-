extern alias P;
using static System.Console;
using P.Prop;

class Test{
	static void Main(){
		Par good = new Par();
		good.Price = 12;
		good.Weight = 2;
		//good.Value = 6.2M;
		WriteLine($"{good.Price}\t{good.Weight}\t{good.Value}");		
	}	
}