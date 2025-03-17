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

print("200 случайных значений из биномиального распределения с параметрами n = ", n, f"p = {p: .5f}")
x_binom = rng.binomial(n, p, N)
print(x_binom)
x_bi_save = x_binom.copy().reshape((20, 10))
df = pd.DataFrame(x_bi_save)
df.to_excel('binomial.xlsx')

task(x_binom, 'binomial')