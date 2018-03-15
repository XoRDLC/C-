using GoO = Go.To.One;
using G = Go.To;

namespace Go{
	class Zero{}
	namespace To{
		class One{}
	}
	namespace Please{
		class Two{}
		class Three : Zero {}
		class Four: To.One{}
	}
}

class Ali{
	static void Main(){
		GoO go = new GoO();	
		G.One go2 = new G.One();
	}
}