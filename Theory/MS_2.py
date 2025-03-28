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

print("200 случайных значений из геометрического распределения с параметром ", f"p = {p: .5f}")
x_geom = rng.geometric(p, N) - 1
print(x_geom)
X_geo_save = x_geom.copy().reshape((20, 10))
X_geo_save.sort()
df = pd.DataFrame(X_geo_save)
df.to_excel('geometric.xlsx')

task(x_geom, 'geometric')