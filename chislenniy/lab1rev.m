function lab1rev(x, nmax)
  reverrors = zeros(nmax, 1);
  logreverrors = zeros(nmax, 1);
  for i = 1:nmax
    reverrors(i) = exp(x) - lab1myrevexp(x, i);
    logreverrors(i) = log10(abs(reverrors(i)));
  endfor
  figure;
  plot(1:nmax, reverrors);
  title("errors");
  figure;
  plot(1:nmax, logreverrors);
  title("logerrors");
endfunction
