This is a demo of an attack on DES_CBC by exploiting a padding oracle  
I wrote this code after reading the excellent article on padding oracles at:  
https://blog.gdssecurity.com/labs/2010/9/14/automated-padding-oracle-attacks-with-padbuster.html

The code just tries to decrypt modified ciphertext, and based on an exception ('500 server response') or no exception ('200') decrypts the ciphertext. 
