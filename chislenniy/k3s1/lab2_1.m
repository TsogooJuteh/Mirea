function interpolation_analysis()
    a = -10;
    b = 20;

    f = @(x) sin(x);
    n_values = [5, 10, 15];

    x_plot = linspace(a, b, 1000);
    f_values = f(x_plot);

    max_errors_uniform = zeros(length(n_values),1);
    max_errors_chebyshev = zeros(length(n_values),1);

    % Цвета и стили линий для различных n
    colors = ['r', 'g', 'b'];
    line_styles = {'-', '--', '-.'};

    %% 1. Построение графиков f(x) и P_n(x) для разных n и типов узлов
    figure('Name', 'Исходная функция и интерполирующие полиномы', 'NumberTitle', 'off');
    hold on;
    plot(x_plot, f_values, 'k-', 'LineWidth', 2, 'DisplayName', 'f(x) = sin(x)');

    for idx = 1:length(n_values)
        n = n_values(idx);
        N = n;

        uniform_nodes = linspace(a, b, N);
        y_uniform = f(uniform_nodes);
        P_uniform = polyfit(uniform_nodes, y_uniform, N-1);
        Pn_uniform = polyval(P_uniform, x_plot);
        plot(x_plot, Pn_uniform, 'Color', colors(idx), 'LineStyle', line_styles{1}, 'LineWidth', 1.5, ...
            'DisplayName', sprintf('Uniform n=%d', n));

        %% Чебышёвские узлы
        chebyshev_nodes = 0.5*(a + b) + 0.5*(b - a)*cos(((2*(0:N-1)+1)*pi)/(2*N));
        y_chebyshev = f(chebyshev_nodes);
        P_chebyshev = polyfit(chebyshev_nodes, y_chebyshev, N-1);
        Pn_chebyshev = polyval(P_chebyshev, x_plot);
        plot(x_plot, Pn_chebyshev, 'Color', colors(idx), 'LineStyle', line_styles{2}, 'LineWidth', 1.5, ...
            'DisplayName', sprintf('Chebyshev n=%d', n));

        %% Сохранение максимальных ошибок
        error_uniform = abs(f_values - Pn_uniform);
        error_chebyshev = abs(f_values - Pn_chebyshev);
        max_errors_uniform(idx) = max(error_uniform);
        max_errors_chebyshev(idx) = max(error_chebyshev);
    end

    xlabel('x');
    ylabel('y');
    title('Исходная функция и интерполирующие полиномы');
    legend('Location', 'best');
    grid on;
    hold off;

    figure('Name', 'Логарифм абсолютной ошибки интерполяции', 'NumberTitle', 'off');
    hold on;
    for idx = 1:length(n_values)
        n = n_values(idx);
        N = n;

        uniform_nodes = linspace(a, b, N);
        y_uniform = f(uniform_nodes);
        P_uniform = polyfit(uniform_nodes, y_uniform, N-1);
        Pn_uniform = polyval(P_uniform, x_plot);
        error_uniform = abs(f_values - Pn_uniform);
        semilogy(x_plot, error_uniform, 'Color', colors(idx), 'LineStyle', line_styles{1}, 'LineWidth', 1.5, ...
            'DisplayName', sprintf('Uniform n=%d', n));

        %% Чебышёвские узлы
        chebyshev_nodes = 0.5*(a + b) + 0.5*(b - a)*cos(((2*(0:N-1)+1)*pi)/(2*N));
        y_chebyshev = f(chebyshev_nodes);
        P_chebyshev = polyfit(chebyshev_nodes, y_chebyshev, N-1);
        Pn_chebyshev = polyval(P_chebyshev, x_plot);
        error_chebyshev = abs(f_values - Pn_chebyshev);
        semilogy(x_plot, error_chebyshev, 'Color', colors(idx), 'LineStyle', line_styles{2}, 'LineWidth', 1.5, ...
            'DisplayName', sprintf('Chebyshev n=%d', n));
    end

    xlabel('x');
    ylabel('|f(x) - P_n(x)|');
    title('Логарифм абсолютной ошибки интерполяции');
    legend('Location', 'best');
    grid on;
    hold off;

    figure('Name', 'Максимальная абсолютная ошибка vs Число узлов', 'NumberTitle', 'off');
    hold on;
    N_values = n_values;
    log_max_uniform = log10(max_errors_uniform);
    log_max_chebyshev = log10(max_errors_chebyshev);

    plot(N_values, log_max_uniform, 'o-', 'Color', 'm', 'LineWidth', 2, ...
        'DisplayName', 'Uniform Nodes');
    plot(N_values, log_max_chebyshev, 's--', 'Color', 'c', 'LineWidth', 2, ...
        'DisplayName', 'Chebyshev Nodes');

    xlabel('Число узлов N');
    ylabel('log_{10}(max |f(x) - P_n(x)|)');
    title('Максимальная абсолютная ошибка vs Число узлов');
    legend('Location', 'best');
    grid on;
    hold off;
end


function task2(a, b, n, x_test)
  uniform_nodes = linspace(a, b, n);
  chebyshev_nodes = cos(((2*(0:n-1) + 1) * pi) / (2*n));
  chebyshev_nodes = 0.5 * ((b - a) * chebyshev_nodes + (b + a));
  function result = f(x)
    result = sin(10 * x) ./ (x.^2 + 1);
  endfunction

  y = f(uniform_nodes);

  P = zeros(n, n);
  P(1, :) = y(:);
  for i = 2 : n
    for j = 1 : (n - i + 1)
      P(i, j) = ((x_test - uniform_nodes(j)) * P(i-1, j+1) - (x_test - uniform_nodes(i+j-1)) * P(i-1, j)) / (uniform_nodes(i+j-1) - uniform_nodes(j));
    endfor
  endfor

  Pn_x = P(n, 1);
  fx_test = f(x_test);
  disp(['Interpolated value at x = ', num2str(x_test), ' is ', num2str(Pn_x)]);
  disp(['Actual value at x = ', num2str(x_test), ' is ', num2str(fx_test)]);
  x_plot = linspace(a, b, n);
  f_values = f(x_plot);
  P_values = zeros(1, length(x_plot));
  for k = 1:length(x_plot)
    P_temp = zeros(n, n);
    P_temp(1, :) = y(:);
    for i = 2:n
      for j = 1:(n - i + 1)
        P_temp(i, j) = ((x_plot(k) - uniform_nodes(j)) * P_temp(i-1, j+1) - (x_plot(k) - uniform_nodes(i+j-1)) * P_temp(i-1, j)) / (uniform_nodes(i+j-1) - uniform_nodes(j));
      endfor
    endfor
    P_values(k) = P_temp(n, 1);
  endfor

  plot(x_plot, f_values, 'r-', 'LineWidth', 2);
  hold on;

  plot(x_plot, P_values, 'b-', 'LineWidth', 2);

  xlabel('x');
  ylabel('y');
  legend('f(x)', 'Pn(x)');
  title('Original function vs Interpolated Polynomial');
  grid on;
  hold off;


endfunction




