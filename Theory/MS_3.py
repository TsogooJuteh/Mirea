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

print("200 случайных значений из распределения Пуассона с параметром ", f"lambda = {lamda: .5f}")
x_poisson = rng.poisson(lamda, N)
print(x_poisson)
x_po_save = x_poisson.copy().reshape((20, 10))
df = pd.DataFrame(x_po_save)
df.to_excel('poisson.xlsx')

task(x_poisson, 'poisson')