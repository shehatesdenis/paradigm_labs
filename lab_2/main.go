package main

import (
	"fmt"
	"math"
)

type IPrint interface {
	Print()
}

type Rectangle struct {
	Width  float64
	Height float64
}

func NewRectangle(width, height float64) Rectangle {
	return Rectangle{
		Width:  width,
		Height: height,
	}
}

func (r Rectangle) Area() float64 {
	return r.Width * r.Height
}

func (r Rectangle) String() string {
	return fmt.Sprintf("Прямоугольник [Ширина: %.2f, Высота: %.2f, Площадь: %.2f]", r.Width, r.Height, r.Area())
}

func (r Rectangle) Print() {
	fmt.Println(r.String())
}

type Square struct {
	Rectangle
}

func NewSquare(side float64) Square {
	return Square{
		Rectangle: NewRectangle(side, side),
	}
}

func (s Square) String() string {
	return fmt.Sprintf("Квадрат [Сторона: %.2f, Площадь: %.2f]", s.Width, s.Area())
}

func (s Square) Print() {
	fmt.Println(s.String())
}

type Circle struct {
	Radius float64
}

func NewCircle(radius float64) Circle {
	return Circle{Radius: radius}
}

func (c Circle) Area() float64 {
	return math.Pi * c.Radius * c.Radius
}

func (c Circle) String() string {
	return fmt.Sprintf("Круг [Радиус: %.2f, Площадь: %.2f]", c.Radius, c.Area())
}

func (c Circle) Print() {
	fmt.Println(c.String())
}

func main() {
	rect := NewRectangle(5.0, 10.0)
	sq := NewSquare(4.0)
	circ := NewCircle(3.0)

	shapes := []IPrint{rect, sq, circ}

	for _, shape := range shapes {
		shape.Print()
	}
}
