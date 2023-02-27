#include <iostream>
#include <math.h>

using namespace std;

class Segment {
	private:
		double x1,y1,x2,y2;
	public:
		Segment(double _x1, double _y1, double _x2, double _y2){
			x1=_x1;
			y1=_y1;
			x2=_x2;
			y2=_y2;
		}
		void operator+(double displacement){
			y1+=displacement;
			y2+=displacement;
		}
		void operator-(double displacement){
			y1-=displacement;
			y2-=displacement;
		}
		double length(){
			return sqrt(pow(x2-x1,2)+pow(y2-y1,2));
		}
		void print(){
			cout << "(" << x1 << "," << y1 << ") - (" << x2 << "," << y2 << ")" << endl;
		}
};

int main(){
	Segment seg(0,0,3,4);
	seg.print();
	
	seg + 2.0;
	seg.print();
	
	seg - 2.0;
	seg.print();
	
	double len = seg.length();
	cout<<"Segment length:"<<len<<endl;
	
	return 0;
}
