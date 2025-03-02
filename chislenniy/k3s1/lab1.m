function lab1(x,nmax)
  errors = zeros(nmax, 1);
  logerrors = zeros(nmax, 1);
  for i = 1:nmax
    errors(i) = exp(x) - lab1myexp(x, i)
    logerrors(i) = log10(abs(errors(i)))
  endfor
  figure;
  plot(1:nmax, errors);
  title("errors");
  figure;
  plot(1:nmax, logerrors);
  title("logerrors");
endfunction

