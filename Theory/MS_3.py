from TR_1 import *
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
import math

N = 200 #Sample
V = 165 #Variant

n = 5 + V % 20
p = 0.2 + 0.003 * V
lamda = 1 + ((-1) ** V) * (V * 0.003)

seed = 10 + V
rng = np.random.default_rng(seed=seed)

print("200 случайных значений из экспонентиального распределения с параметром ", f"lambda = {lamda: .5f}")
x_exponential = rng.exponential(1/lamda, N)
print(x_exponential)
x_exp_save = x_exponential.copy().reshape((20, 10))
df1 = pd.DataFrame(x_exp_save)
df1.to_excel('exponential.xlsx')
x_exponential.sort()
x_exp_sorted_save = x_exponential.copy().reshape((20, 10))
df = pd.DataFrame(x_exp_sorted_save)
df.to_excel('exponential_sorted.xlsx')

task(x_exponential, 'exponential')