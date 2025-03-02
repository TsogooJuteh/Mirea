function lab1(x,nmax)
  errors = zeros(nmax, 1);
  for i = 1:nmax
    errors(i) = exp(x) - lab1myexp(x, i)
  endfor
  plot(1:nmax, errors)
endfunction
