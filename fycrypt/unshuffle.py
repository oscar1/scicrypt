#!/usr/bin/python

# get the 'random' inputbytes of a FY shuffle (Durstenfeld variant), given the final list and assuming '[0,1,2,3,..,n]' as the initial list.

# example:
# python unshuffle.py '[7,6,5,4,3,2,1,0]'
# [0, 1, 2, 3, 3, 2, 1]

import sys

# eval is evil but also convenient. 
# tbox holds the list we want to find the inputbytes for
tbox=eval(sys.argv[1])

deck=range(len(tbox))

randnums=[]
i=len(tbox)-1

while len(randnums) < len(tbox)-1:
	randnum=deck.index(tbox[i]) 
	randnums.append(randnum)
	deck[randnum]=deck[i]
	deck[i]=tbox[i]
	i=i-1

print(randnums)
