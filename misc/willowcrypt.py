#!/bin/python
import sys

def repsqr(a,n,m): # repeated squaring a**n mod m
    if n==1: 
            return a
    if n==0: 
            return 1
    return (repsqr(a,n%2,m)*repsqr(a,n/2,m)**2) % m

def willowprng(P,S):
    Q=(P-1)/2
    Y = repsqr(3,(2* S* (Q - 1)), P)
    while True:
        yield int(((Y >> 1) + Y) % 2)
        Y=(Y*3)%P

def willowcrypt(P,S,plain):
    c=""
    x=willowprng(P,S)
    for p in plain:
        c=c+chr(ord(p) ^ sum(j*2**i for (i,j) in zip(range(7,-1,-1),[x.next() for i in range(8)])))
    return c

p=open(sys.argv[3]).read()
c=willowcrypt(int(sys.argv[1]),int(sys.argv[2]),p)
print c,



