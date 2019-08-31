#!/usr/bin/python

# naive bruteforcer of prng. Takes as input a list of known generated 'random' numbers and returns the first seed which works.
# (perhaps get a list by running unshuffle.py first on a known T-box)
# the second argument is the number of random numbers to check (so it doesn't check the full list).

# python bruteprng.py '[5,11,14,11,11,11, 8, 5, 0, 3, 0, 5, 3, 3, 0, 1]' 8
# 21677946 

import sys

rands=eval(sys.argv[1])
numstocheck=int(sys.argv[2])

m=len(rands)
# the only 'optimization' is that we can increase the candidates by the length of the list on each try (because of how the numbers were generated to be used in FY shuffle)
firstmod=len(rands)+1
firstrand=rands[0]

CIS_A=0x41C64E6D
CIS_C=0x3039
CISPRNG_RAND_MAX=0x7FFFFFFF

def f(x):
	return (CIS_A*x + CIS_C)&CISPRNG_RAND_MAX


x=firstrand
while  x < CISPRNG_RAND_MAX: 
	oldx=x
	i=1
	while True:
		x1=f(x)
		ceil=(CISPRNG_RAND_MAX/(firstmod-i))*(firstmod-i)
		while x1 > ceil:
			x=x1
			x1=f(x)
			print("oops")
		if x1%(firstmod-i) == rands[i]:
			i=i+1
			x=x1
			if i>numstocheck:
				print(oldx)
				exit()
		else:
			x=oldx
			break
	x=x+firstmod

print(x)
