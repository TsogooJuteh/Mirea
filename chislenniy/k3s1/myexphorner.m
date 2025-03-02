function r2 = myexphorner(x, nmax)
  r2 = 1;
  for i = nmax:-1:1
    r2 = r2 + x^i/factorial(i);
  endfor
  disp(r2);
endfunction
