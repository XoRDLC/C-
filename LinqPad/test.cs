using Go.To.Please;

namespace Go.To.Please{

	class GoX{
		private int x;
		public GoX(int x){this.x = x;}
		public int GetX(){return x;}
	}
	class Stay{
		private int y;
		public Stay(int y){this.y = y;}
		public int GetY(){return y;}
	}
	class Print{
		public void print(object s){System.Console.WriteLine(s.ToString());}
	}
}

class Test{
	static void Main(){
		GoX go = new GoX(10);
		Stay stay = new Stay(20);
		Print print = new Print();
		
		print.print(go.GetX());
		print.print(stay.GetY());
	}
}
