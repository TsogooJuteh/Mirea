function r = lab1myexp(x, nmax)
  r = 0;
  for i = 0:nmax - 1
    temp = x^i/factorial(i);
    r = r + temp;
  endfor
  disp(r);
endfunction
