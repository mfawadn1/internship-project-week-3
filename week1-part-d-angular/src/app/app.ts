import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Student } from './student.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: '../styles.css'
})
export class App {
  userName = 'Asiya';
  internshipTitle = 'AI Software Development Internship';

  selectedStudent: Student | null = null;

  students: Student[] = [
    { id: 1, name: 'Asiya', department: 'Artificial Intelligence', grade: 'A', email: 'alice@example.com', imageUrl: 'https://i.pravatar.cc/150?img=5' },
    { id: 2, name: 'Minahil', department: 'Mathematics', grade: 'B', email: 'bob@example.com', imageUrl: 'https://i.pravatar.cc/150?img=11' },
    { id: 3, name: 'Aliha', department: 'Physics', grade: 'A', email: 'charlie@example.com', imageUrl: 'https://i.pravatar.cc/150?img=6' },
    { id: 4, name: 'Anoosha', department: 'Engineering', grade: 'A+', email: 'diana@example.com', imageUrl: 'https://i.pravatar.cc/150?img=9' },
    { id: 5, name: 'Ayema', department: 'Computer Science', grade: 'B+', email: 'eve@example.com', imageUrl: 'https://i.pravatar.cc/150?img=1' }
  ];

  selectStudent(student: Student) {
    this.selectedStudent = student;
  }
}
