library("lmtest")
library("GGally")

data = swiss
help(swiss)

data
summary(data)
ggpairs(data)

mean(swiss$Agriculture)
var(swiss$Agriculture)
sd(swiss$Agriculture)
mean(swiss$Examination)
var(swiss$Examination)
sd(swiss$Examination)

model1 <- lm(Infant.Mortality~Agriculture, data = swiss)
plot(model1)
ggpairs(model1)
model2 <- lm(Infant.Mortality~Examination, data = swiss)
plot(model2)
ggpairs(model2)

summary(model1)$r.squared
summary(model2)$r.squared

summary(model1)$coefficients
summary(model2)$coefficients
