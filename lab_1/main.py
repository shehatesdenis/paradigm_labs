import sys
import math


def get_coef(index, prompt):
    while True:
        try:
            coef_str = sys.argv[index]
        except IndexError:
            print(prompt)
            coef_str = input()

        try:
            return float(coef_str)
        except ValueError:
            print("Некорректный ввод. Введите действительное число.")
            if len(sys.argv) > index:
                sys.argv[index] = "invalid"


print("Программа для решения биквадратных уравнений")

a = get_coef(1, "Введите коэффициент A: ")
b = get_coef(2, "Введите коэффициент B: ")
c = get_coef(3, "Введите коэффициент C: ")

d = b ** 2 - 4 * a * c

if d < 0:
    print("Нет корней")
else:
    roots = []
    if d == 0:
        t_values = [-b / (2 * a)]
    else:
        t_values = [
            (-b - math.sqrt(d)) / (2 * a),
            (-b + math.sqrt(d)) / (2 * a)
        ]

    for t in t_values:
        if t > 0:
            roots.append(math.sqrt(t))
            roots.append(-math.sqrt(t))
        elif t == 0:
            roots.append(0.0)

    roots = sorted(list(set(roots)))

    if len(roots) == 0:
        print("Нет корней")
    elif len(roots) == 1:
        print("Один корень:", roots[0])
    elif len(roots) == 2:
        print("Два корня:", roots[0], "и", roots[1])
    elif len(roots) == 3:
        print("Три корня:", roots[0], ",", roots[1], "и", roots[2])
    elif len(roots) == 4:
        print("Четыре корня:", roots[0], ",", roots[1], ",", roots[2], "и", roots[3])