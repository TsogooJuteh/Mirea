function results = neville_interpolation(a, b)
    % Define the number of interpolation points
    n_values = [10, 20, 30];
    results = struct();
    colors = ['r', 'g', 'b'];

    % Generate points for plotting the original function
    x_plot = linspace(a, b, 1000);
    y_original = sin(x_plot);  % Corrected to y = sin(x)

    % Define Chebyshev points generator
    function x_chebyshev = chebyshev(n)
        x_chebyshev_standard = cos((2 * (1:n) - 1) / (2 * n) * pi);
        x_chebyshev = (b - a) / 2 * x_chebyshev_standard + (a + b) / 2;
    end

    %% 1. Plot the Original Function and Interpolation Polynomials (Equally Spaced Points)
    figure;
    hold on;
    plot(x_plot, y_original, 'k', 'LineWidth', 1.5, 'DisplayName', 'Original Function');

    for idx = 1:length(n_values)
        n = n_values(idx);
        x = linspace(a, b, n)';
        y = sin(x);  % Changed to y = sin(x)

        % Construct the Vandermonde matrix for polynomial fitting
        phi = ones(n, n + 1);
        for i = 2:n+1
            phi(:, i) = x.^(i-1);
        end
        phi_T = phi';
        left_side = phi_T * phi;
        right_side = phi_T * y;
        c = left_side \ right_side;
        results(idx).c = c;

        fprintf('Results for n = %d:\n', n);
        disp('Coefficients c:');
        disp(c);
        fprintf('\n');

        % Evaluate the polynomial at x_plot
        y_plot = zeros(size(x_plot));
        for i = 0:n
            y_plot = y_plot + c(i+1) * x_plot.^i;
        end
        plot(x_plot, y_plot, colors(idx), 'LineWidth', 1.5, 'DisplayName', sprintf('Polynomial (n=%d)', n));
    end

    title('Interpolating Polynomials and Original Function (Equally Spaced Points)');
    xlabel('x');
    ylabel('Function Value');
    legend('show');
    hold off;

    %% 2. Plot the Original Function and Interpolation Polynomials (Chebyshev Points)
    figure;
    hold on;
    plot(x_plot, y_original, 'k', 'LineWidth', 1.5, 'DisplayName', 'Original Function');

    for idx = 1:length(n_values)
        n = n_values(idx);
        x = chebyshev(n)';
        y = sin(x);  % Changed to y = sin(x)

        % Construct the Vandermonde matrix for polynomial fitting
        phi = ones(n, n + 1);
        for i = 2:n+1
            phi(:, i) = x.^(i-1);
        end
        phi_T = phi';
        left_side = phi_T * phi;
        right_side = phi_T * y;
        c = left_side \ right_side;
        results(idx).c = c;

        fprintf('Results for n = %d (Chebyshev):\n', n);
        disp('Coefficients c:');
        disp(c);
        fprintf('\n');

        % Evaluate the polynomial at x_plot
        y_plot = zeros(size(x_plot));
        for i = 0:n
            y_plot = y_plot + c(i+1) * x_plot.^i;
        end
        plot(x_plot, y_plot, colors(idx), 'LineWidth', 1.5, 'DisplayName', sprintf('Polynomial (n=%d)', n));
    end

    title('Interpolating Polynomials and Original Function (Chebyshev Points)');
    xlabel('x');
    ylabel('Function Value');
    legend('show');
    hold off;

    %% 3. Plot Log10 of Absolute Error for Interpolation Polynomials (Equally Spaced Points)
    figure;
    hold on;
    for idx = 1:length(n_values)
        n = n_values(idx);
        x = linspace(a, b, n)';
        y = sin(x);  % Changed to y = sin(x)

        % Construct the Vandermonde matrix for polynomial fitting
        phi = ones(n, n + 1);
        for i = 2:n+1
            phi(:, i) = x.^(i-1);
        end
        phi_T = phi';
        left_side = phi_T * phi;
        right_side = phi_T * y;
        c = left_side \ right_side;

        % Evaluate the polynomial at x_plot
        y_plot = zeros(size(x_plot));
        for i = 0:n
            y_plot = y_plot + c(i+1) * x_plot.^i;
        end

        % Compute log10 of absolute error
        abs_error = abs(y_original - y_plot);
        % To avoid log10(0), set a minimum threshold
        abs_error(abs_error < 1e-12) = 1e-12;
        log10_abs_error = log10(abs_error);

        plot(x_plot, log10_abs_error, colors(idx), 'LineWidth', 1.5, 'DisplayName', sprintf('Polynomial (n=%d)', n));
    end

    title('Log_{10} of Absolute Error for Interpolation Polynomials (Equally Spaced Points)');
    xlabel('x');
    ylabel('Log_{10} Absolute Error');
    legend('show');
    hold off;

    %% 4. Plot Log10 of Absolute Error for Interpolation Polynomials (Chebyshev Points)
    figure;
    hold on;
    for idx = 1:length(n_values)
        n = n_values(idx);
        x = chebyshev(n)';
        y = sin(x);  % Changed to y = sin(x)

        % Construct the Vandermonde matrix for polynomial fitting
        phi = ones(n, n + 1);
        for i = 2:n+1
            phi(:, i) = x.^(i-1);
        end
        phi_T = phi';
        left_side = phi_T * phi;
        right_side = phi_T * y;
        c = left_side \ right_side;

        % Evaluate the polynomial at x_plot
        y_plot = zeros(size(x_plot));
        for i = 0:n
            y_plot = y_plot + c(i+1) * x_plot.^i;
        end

        % Compute log10 of absolute error
        abs_error = abs(y_original - y_plot);
        % To avoid log10(0), set a minimum threshold
        abs_error(abs_error < 1e-12) = 1e-12;
        log10_abs_error = log10(abs_error);

        plot(x_plot, log10_abs_error, colors(idx), 'LineWidth', 1.5, 'DisplayName', sprintf('Polynomial (n=%d)', n));
    end

    title('Log_{10} of Absolute Error for Interpolation Polynomials (Chebyshev Points)');
    xlabel('x');
    ylabel('Log_{10} Absolute Error');
    legend('show');
    hold off;

    %% 5. Plot Log10 of Maximum Absolute Error vs Polynomial Degree
    max_errors_even = zeros(length(12), 1);
    max_errors_chebyshev = zeros(length(12), 1);

    for d = 1:12
        % Evenly spaced points
        x = linspace(a, b, d+1)';
        y = sin(x);  % Changed to y = sin(x)

        phi = ones(d+1, d+1);
        for i = 2:d+1
            phi(:, i) = x.^(i-1);
        end

        phi_T = phi';
        left_side = phi_T * phi;
        right_side = phi_T * y;
        c = left_side \ right_side;

        y_plot = zeros(size(x_plot));
        for i = 0:d
            y_plot = y_plot + c(i+1) * x_plot.^i;
        end

        max_errors_even(d) = log10(max(abs(y_original - y_plot)));
    end

    for d = 1:12
        % Chebyshev points
        x = chebyshev(d+1)';
        y = sin(x);  % Changed to y = sin(x)

        phi = ones(d+1, d+1);
        for i = 2:d+1
            phi(:, i) = x.^(i-1);
        end

        phi_T = phi';
        left_side = phi_T * phi;
        right_side = phi_T * y;
        c = left_side \ right_side;

        y_plot = zeros(size(x_plot));
        for i = 0:d
            y_plot = y_plot + c(i+1) * x_plot.^i;
        end

        max_errors_chebyshev(d) = log10(max(abs(y_original - y_plot)));
    end

    degrees = 1:12;
    figure;
    hold on;
    plot(degrees, max_errors_even, '-r', 'LineWidth', 1.5, 'DisplayName', 'Evenly Spaced Points');
    plot(degrees, max_errors_chebyshev, '-b', 'LineWidth', 1.5, 'DisplayName', 'Chebyshev Points');
    title('Log_{10} of Maximum Absolute Error vs Polynomial Degree');
    xlabel('Polynomial Degree (d)');
    ylabel('Log_{10} Maximum Absolute Error');
    legend('show');
    grid on;
    hold off;
end

