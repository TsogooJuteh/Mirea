import numpy as np
import matplotlib.pyplot as plt
from scipy import stats
import pandas as pd
import math

N = 200 #Sample
V = 165 #Variant

n = 5 + V % 20
p = 0.2 + 0.003 * V
lamda = 1 + ((-1) ** V) * (V * 0.003)

seed = 10+V
rng = np.random.default_rng(seed=seed)


def task(X_distribution, name_distribution):
  #1
  counts = np.unique(X_distribution, return_counts=True)
  X = counts[0]
  freq = counts[1]
  rel_freq = counts[1] / N
  cum_freq = np.cumsum(rel_freq)

  total_freq = freq.sum(axis=0)
  total_rel_freq = rel_freq.sum(axis=0)
  intervals = None

  if name_distribution == 'binomial' or name_distribution == 'geometric' or name_distribution == 'poisson':
    total_row = {
      'X': 'Total',
      'Frequency': total_freq,
      'Relative Frequency': total_rel_freq,
      'Cumulative Frequency': ''
    }

    df_freq = pd.DataFrame({
        'X': counts[0],
        'Frequency': [f"{val:.5f}" for val in freq],
        'Relative Frequency': [f"{val:.5f}" for val in rel_freq],
        'Cumulative Frequency': [f"{val:.5f}" for val in cum_freq]
        })
    df_freq.loc[len(df_freq)] = total_row

    print(df_freq)
    df_freq.to_excel(name_distribution + '_stat.xlsx')

  if name_distribution == 'poisson':
        step = 1 + np.log2(N)
        distance_between_step = (X.max() - X.min()) / step
        intervals = np.array([X.min() + i * distance_between_step for i in range(int(step))])
        intervals = np.append(intervals, X.max())
        intervals_freq = np.zeros(len(intervals)-1)
        intervals_relative_freq = np.zeros(len(intervals)-1)
        for i in range(len(intervals)-1):
            for j in range(len(X)):
                if intervals[i] <= X[j] < intervals[i+1]:
                    intervals_freq[i] += freq[j]
                    intervals_relative_freq[i] += rel_freq[j]

        total_row = {'Frequency': total_freq, 'Relative Frequency': total_rel_freq}
        df_freq_intervals = pd.DataFrame({'Interval: ': zip([f"{val:.5f}" for val in intervals[:-1]], [f"{val:.5f}" for val in intervals[1:]]), 'Frequency': [f"{val:.5f}" for val in intervals_freq], 'Relative Frequency': [f"{val:.5f}" for val in intervals_relative_freq]})
        df_freq_intervals.loc[len(df_freq_intervals)] = total_row
        print(df_freq_intervals)
        df_freq_intervals.to_excel(name_distribution + '_intervals.xlsx')

        middle = np.array([(intervals[i] + intervals[i+1]) / 2 for i in range(len(intervals)-1)])
        df_freq_middle = pd.DataFrame({'Middle': [f"{val:.5f}" for val in middle], 'Frequency': [f"{val:.5f}" for val in intervals_freq], 'Relative Frequency': [f"{val:.5f}" for val in intervals_relative_freq]})
        df_freq_middle.loc[len(df_freq_intervals)] = total_row
        print(df_freq_middle)
        df_freq_middle.to_excel(name_distribution + '_middle.xlsx')

  #2
  if name_distribution == 'binomial':
    theo_freq = np.array([(math.comb(n, k) * (p ** k) * ((1-p) ** (n-k))) for k in X])
  elif name_distribution == 'geometric':
    theo_freq = np.array([(p * ((1-p) ** (k-1))) for k in counts[0]])
  elif name_distribution == 'poisson':
    theo_freq = np.array([(lamda ** k) * np.exp(-lamda) / math.factorial(k) for k in counts[0]])
    intervals_theoretical_relative_freq = np.zeros(len(intervals)-1)
    for i in range(len(intervals)-1):
        for j in range(len(X)):
            if intervals[i] <= X[j] < intervals[i+1]:
                intervals_theoretical_relative_freq[i] += theo_freq[j]

  #plot
  if name_distribution == 'binomial' or name_distribution == 'geometric':
    plt.figure(figsize = (8,6))
    plt.plot(X, rel_freq, marker = 'o', linestyle = '-', color = 'b', label = 'Relative Frequency')
    plt.plot(X, theo_freq, marker = 'o', linestyle = '--', color = 'red', label = 'Theoretical Probabilities')
    plt.xlabel("Value")
    plt.ylabel("Frequency")
    plt.title(name_distribution + "Distribution")
    plt.legend()
    plt.grid(True)
    plt.show()
    #3
    plt.figure(figsize = (8,6))
    for i in range(0, len(counts[0])-1):
      plt.plot([counts[0][i], counts[0][i+1]], [cum_freq[i], cum_freq[i]], color='b', linestyle='-', linewidth=2)
    plt.grid(True)
    plt.show()
  if name_distribution == 'poisson':
    fig, ax1 = plt.subplots(figsize=(8, 6))
    
    #ax1.plot(counts[0], relative_freq, marker='o', linestyle='-', color='b', label='Relative Frequency')
    ax1.plot(counts[0], theo_freq, marker='o', linestyle='-', color='r', label='Theoretical Frequency')
    ax1.set_xlabel('Value')
    ax1.set_ylabel('Frequency (Line)')
    ax1.set_title('Histogram')
    ax1.legend(loc='upper left')
    ax1.grid(True)
    ax2 = ax1.twinx()
    ax2.hist(X_distribution, bins='sturges', edgecolor='black', alpha=0.5, label='Histogram')
    ax2.set_ylabel('Count (Histogram)')
    ax2.legend(loc='upper right')
    plt.show()
    
    plt.figure(figsize = (8,6))
    for i in range(0, len(counts[0])-1):
      plt.plot([counts[0][i], counts[0][i+1]], [cum_freq[i], cum_freq[i]], color='b', linestyle='-', linewidth=2)
    plt.grid(True)
    plt.show()

  #4
  mean = np.array([X[i] * rel_freq[i] for i in range(len(X))]).sum()
  print("Sample mean = ", f"{mean: .5f}")

  #5
  variance = np.array([((X[i] - mean) ** 2) * rel_freq[i] for i in range(len(X))]).sum()
  print("Sample variance = ", f"{variance: .5f}")

  #6
  deviation = np.sqrt(variance)
  print("Sample standard deviation = ", f"{deviation: .5f}")

  #7
  modes = X[np.argwhere(freq == np.amax(freq))].flatten().tolist()
  mode = (modes[0] + modes[len(modes) - 1]) / 2
  print("Mode = ", f"{mode: .5f}")

  #8
  if len(X) % 2 == 0:
      median = (X[int(len(X)/2)] + X[int(len(X)/2) - 1]) / 2
  else:
      if 0.5 in X.tolist():
          median = X[cum_freq.tolist().index(0.5)]
      else:
          left = -1
          for element in cum_freq:
              if element < 0.5:
                  left += 1
              else:
                  right = left + 1
                  break
          median = (X[left] + X[right]) / 2
  print("Median = ", f"{median: .5f}")

  #9
  def sample_k_moment_around_mean(k, mean):
      return np.array([(X[i] - mean) ** k * rel_freq[i] for i in range(len(X))]).sum()
  sample_skeness = sample_k_moment_around_mean(3, mean) / deviation ** 3
  print("Sample skewness = ", f"{sample_skeness: .5f}")
  sample_kurtosis = sample_k_moment_around_mean(4, mean) / deviation ** 4 - 3
  print("Sample kurtosis = ", f"{sample_kurtosis: .5f}")

  abs_diff_freq = np.abs(theo_freq - rel_freq)
  freq_compare = pd.DataFrame({
      'X': X,
      'Relative Frequency': [f"{val:.5f}" for val in rel_freq],
      'Theoretical Frequency': [f"{val:.5f}" for val in theo_freq],
      'Absolute Difference': [f"{val:.5f}" for val in abs_diff_freq]
      })
  
  if name_distribution == 'poisson': 
    abs_diff_freq_intervals = np.abs(intervals_theoretical_relative_freq - intervals_relative_freq)
    freq_compare_intervals = pd.DataFrame({
        'Interval: ': zip([f"{val:.5f}" for val in intervals[:-1]], [f"{val:.5f}" for val in intervals[1:]]),
        'Relative Frequency': [f"{val:.5f}" for val in intervals_relative_freq], 
        'Theoretical Frequency': [f"{val:.5f}" for val in intervals_theoretical_relative_freq], 
        'Absolute Difference': [f"{val:.5f}" for val in abs_diff_freq_intervals]
        })
    total_row_compare_intervals = {
        'Relative Frequency': total_rel_freq,
        'Theoretical Frequency': 1,
        'Absolute Difference': np.max(abs_diff_freq)
    }
    freq_compare_intervals.loc[len(freq_compare_intervals)] = total_row_compare_intervals
    freq_compare_intervals.to_excel(name_distribution + '_intervals_compare.xlsx')

  total_row_compare = {
      'X': 'Total',
      'Relative Frequency': f"{total_rel_freq: .5f}",
      'Theoretical Frequency': f"{1: .5f}",
      'Absolute Difference': f"{np.max(abs_diff_freq): .5f}"
      }
  freq_compare.loc[len(freq_compare)] = total_row_compare
  print(freq_compare)
  freq_compare.to_excel(name_distribution + '_compare.xlsx')

  #10
  if name_distribution == 'binomial':
    theo_mean = n * p
    theo_var = n * p * (1 - p)
    theo_deviation = np.sqrt(n * p * (1 - p))
    theo_skewness = ((1-p)-p) / np.sqrt(n * p * (1 - p))
    theo_kurtois = (1 - 6 * p * (1 - p)) / (n * p * (1 - p))
    theo_mode = np.floor((n + 1) * p)
    theo_median = np.floor(n * p)
    theo_values = np.array([theo_mean, theo_var, theo_deviation, theo_skewness, theo_kurtois, theo_mode, theo_median])
    real_values = np.array([mean, variance, deviation, sample_skeness, sample_kurtosis, mode, median])
    abs_differences = np.array([abs(mean - theo_mean), abs(variance - theo_var), abs(deviation - theo_deviation), abs(sample_skeness - theo_skewness), abs(sample_kurtosis - theo_kurtois), abs(mode - theo_mode), abs(median - theo_median)])
    rel_differences = np.array([abs_differences[i] / theo_values[i] for i in range(len(theo_values))])
    char_combine = pd.DataFrame({'Characteristic': ['Mean', 'Variance', 'Deviation', 'Skewness', 'Kurtosis', 'Mode', 'Median'],
                                'Sample': [f"{val:.5f}" for val in real_values],
                                'Theoretical': [f"{val:.5f}" for val in theo_values],
                                'Absolute Difference': [f"{val:.5f}" for val in abs_differences],
                                'Relative Difference': [f"{val:.5f}" for val in rel_differences]
                                })
    char_combine.to_excel(name_distribution + '_char.xlsx')
  elif name_distribution == 'geometric':
    theo_mean = 1 / p
    theo_var = (1 - p) / (p ** 2)
    theo_deviation = np.sqrt((1 - p) / (p ** 2))
    theo_skewness = (2 - p) / np.sqrt(1 - p)
    theo_kurtois = 6 + p ** 2 / (1 - p)
    theo_mode = 1
    theo_median = np.round((-1 / np.log2(1 - p)))
    theo_values = np.array([theo_mean, theo_var, theo_deviation, theo_skewness, theo_kurtois, theo_mode, theo_median])
    real_values = np.array([mean, variance, deviation, sample_skeness, sample_kurtosis, mode, median])
    abs_differences = np.array([abs(real_values[i] - theo_values[i]) for i in range(len(theo_values))])
    rel_differences = np.array([abs_differences[i] / theo_values[i] for i in range(len(theo_values))])
    char_combine = pd.DataFrame({'Characteristic': ['Mean', 'Variance', 'Deviation', 'Skewness', 'Kurtosis', 'Mode', 'Median'],
                                'Sample': [f"{val:.5f}" for val in real_values],
                                'Theoretical': [f"{val:.5f}" for val in theo_values],
                                'Absolute Difference': [f"{val:.5f}" for val in abs_differences],
                                'Relative Difference': [f"{val:.5f}" for val in rel_differences]
                                })
    char_combine.to_excel(name_distribution + '_char.xlsx')
  elif name_distribution == 'poisson':
    theo_mean = lamda
    theo_var= lamda
    theo_deviation = np.sqrt(lamda)
    theo_skewness = 1 / np.sqrt(lamda)
    theo_kurtois = 1 / lamda
    theo_mode = np.floor(lamda)
    theo_median = np.floor(lamda + 1/3 - 0.02/lamda)
    theo_values = np.array([theo_mean, theo_var, theo_deviation, theo_skewness, theo_kurtois, theo_mode, theo_median])
    real_values = np.array([mean, variance, deviation, sample_skeness, sample_kurtosis, mode, median])
    abs_differences = np.array([abs(real_values[i] - theo_values[i]) for i in range(len(theo_values))])
    rel_differences = []
    for i in range(len(theo_values)):
        if theo_values[i] == 0:
            rel_differences.append(np.nan)  # or "N/A"
        else:
            rel_differences.append(abs_differences[i] / theo_values[i])
    rel_differences = np.array(rel_differences)
    char_combine = pd.DataFrame({'Characteristic': ['Mean', 'Variance', 'Deviation', 'Skewness', 'Kurtosis', 'Mode', 'Median'],
                                'Sample': [f"{val:.5f}" for val in real_values],
                                'Theoretical': [f"{val:.5f}" for val in theo_values],
                                'Absolute Difference': [f"{val:.5f}" for val in abs_differences],
                                'Relative Difference': [f"{val:.5f}" for val in rel_differences]
                                })
    char_combine.to_excel(name_distribution + '_char.xlsx')
    


  print(char_combine)
