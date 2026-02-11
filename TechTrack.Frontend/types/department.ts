// src/types/department.ts
import { Equipment } from "./equipment";

export interface Department {
  id: string;
  name: string;
  equipments: Equipment[];
}
