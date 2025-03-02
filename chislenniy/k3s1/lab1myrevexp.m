function r1 = lab1myrevexp(x, nmax)
  r1 = 0;
  for i = nmax:-1:0
    temp = x ^ i / factorial(i);
    r1 = r1 + temp;
  endfor
  disp(r1);
endfunction
