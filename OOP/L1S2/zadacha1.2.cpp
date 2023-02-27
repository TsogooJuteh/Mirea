#include <iostream>
#include <math.h>

using namespace std;

class Segment{
	public:
		double x1,y1,x2,y2;
		Segment(double _x1, double _y1, double _x2, double _y2){
			x1=_x1;
			y1=_y1;
			x2=_x2;
			y2=_y2;
		}
		Segment operator+(double up){
			return Segment(x1+up, y1+up, x2+up, y2+up);
		}
		Segment operator-(double down){
			return Segment(x1-down, y1-down, x2-down, y2-down);
		}
		double dist(){
			double midx=(x1+x2)/2.0;
			double midy=(y1+y2)/2.0;
			return sqrt(midx*midx+midy*midy);
		}
		void print(){
			cout<<"("<<x1<<","<<y1<<") ("<<x2<<","<<y2<<")"<<endl; 
		}
};

int main(){
	Segment seg(0,0,3,4);
	seg.print();
	
	Segment seg1 = seg + 2.0;
	cout << "(" << seg1.x1 << "," << seg1.y1 << ") (" << seg1.x2 << "," << seg1.y2 << ")" <<endl;
	
	Segment seg2 = seg - 1.5;
	cout << "(" << seg2.x1 << "," << seg2.y1 << ") (" << seg2.x2 << "," << seg2.y2 << ")" <<endl;
	
	double dist=seg.dist();
	cout<<dist<<endl;
	
	return 0;
}
