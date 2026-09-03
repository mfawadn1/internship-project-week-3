import { Injectable } from '@angular/core';
import { Student } from './student.model';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private mockStudents: Student[] = [
    { id: 1, name: 'Asiya', email: 'asiya@example.com', department: 'CS' },
    { id: 2, name: 'Alice', email: 'alice@example.com', department: 'IT' }
  ];

  private studentsSubject = new BehaviorSubject<Student[]>(this.mockStudents);
  students$ = this.studentsSubject.asObservable();

  constructor() { }

  getStudents(): Observable<Student[]> {
    return this.students$;
  }

  addStudent(student: Omit<Student, 'id'>): void {
    const newStudent = { ...student, id: this.mockStudents.length + 1 };
    this.mockStudents = [...this.mockStudents, newStudent];
    this.studentsSubject.next(this.mockStudents);
  }
}
