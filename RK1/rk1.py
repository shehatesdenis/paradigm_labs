from operator import itemgetter


class Driver:
    """Водитель"""
    def __init__(self, id, surname, salary, park_id):
        self.id = id
        self.surname = surname
        self.salary = salary
        self.park_id = park_id


class Park:
    """Автопарк"""
    def __init__(self, id, name):
        self.id = id
        self.name = name


class DriverPark:
    """
    Связь многие-ко-многим 'Водители - Автопарки'
    """
    def __init__(self, park_id, driver_id):
        self.park_id = park_id
        self.driver_id = driver_id


# Автопарки
parks = [
    Park(1, 'Автопарк Авангард'),
    Park(2, 'Автопарк Берег'),
    Park(3, 'Автопарк Весна'),
    Park(4, 'ГорЭлектроТранс'),
]

# Водители
drivers = [
    Driver(1, 'Штыков', 45000, 1),
    Driver(2, 'Иванов', 35000, 1),
    Driver(3, 'Петров', 40000, 2),
    Driver(4, 'Сидорова', 30000, 3),
    Driver(5, 'Кузнецов', 50000, 1),
    Driver(6, 'Алексеев', 42000, 4),
]

# Связь многие-ко-многим
drivers_parks = [
    DriverPark(1, 1),
    DriverPark(4, 1),
    DriverPark(1, 2),
    DriverPark(2, 2),
    DriverPark(2, 3),
    DriverPark(3, 3),
    DriverPark(3, 4),
    DriverPark(1, 5),
    DriverPark(4, 6),
    DriverPark(1, 6),
]


def main():
    """Основная функция"""

    # Соединение данных один-ко-многим (Автопарк - Водитель)
    one_to_many = [(d.surname, d.salary, p.name)
                   for p in parks
                   for d in drivers
                   if d.park_id == p.id]

    # Соединение данных многие-ко-многим
    many_to_many_temp = [(p.name, dp.park_id, dp.driver_id)
                         for p in parks
                         for dp in drivers_parks
                         if p.id == dp.park_id]

    many_to_many = [(d.surname, d.salary, dep_name)
                    for dep_name, dep_id, driver_id in many_to_many_temp
                    for d in drivers if d.id == driver_id]

    print('Задание Д1 (один-ко-многим)')
    print('Список всех водителей, у которых фамилия заканчивается на "ов", и названия их автопарков:')
    res_d1 = [(surname, name) for surname, salary, name in one_to_many
              if surname.endswith('ов')]
    print(res_d1)

    print('\nЗадание Д2 (один-ко-многим)')
    print('Список автопарков со средней зарплатой водителей в каждом автопарке, отсортированный по средней зарплате:')
    res_d2_unsorted = []
    # Перебираем все автопарки
    for p in parks:
        # Список водителей автопарка
        p_drivers = list(filter(lambda i: i[2] == p.name, one_to_many))
        # Если автопарк не пустой
        if len(p_drivers) > 0:
            # Зарплаты водителей автопарка
            p_sals = [sal for _, sal, _ in p_drivers]
            # Средняя зарплата водителей автопарка
            p_avg = sum(p_sals) / len(p_sals)
            res_d2_unsorted.append((p.name, p_avg))
    # Сортировка по средней зарплате
    res_d2 = sorted(res_d2_unsorted, key=itemgetter(1), reverse=True)
    print(res_d2)

    print('\nЗадание Д3 (многие-ко-многим)')
    print('Список всех автопарков, у которых название начинается с буквы "А", и список работающих в них водителей:')
    res_d3 = {}
    # Перебираем все автопарки
    for p in parks:
        if p.name.startswith('А'):
            # Список водителей автопарка
            p_drivers = list(filter(lambda i: i[2] == p.name, many_to_many))
            # Только ФИО водителей
            p_drivers_names = [x for x, _, _ in p_drivers]
            # Добавляем результат в словарь
            # ключ - автопарк, значение - список фамилий
            res_d3[p.name] = p_drivers_names
    print(res_d3)


if __name__ == '__main__':
    main()
