# scicrypt
Some random snippets of sci.crypt challenge code


Poor mans histogram using grep and friends:

 xxd -p  bla.txt.bin | egrep -o .. | sort | uniq -c | sort -n
      1 0d
      1 28
      1 39
      1 51
      1 5c
      1 88
      1 90
      1 99
      1 b9
      1 e2
      1 e4
      1 e6
      1 f7
      2 1c
      2 57
    503 48
    504 05
