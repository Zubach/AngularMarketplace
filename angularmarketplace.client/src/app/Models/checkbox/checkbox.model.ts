export interface Checkbox {
    id?:number;
    title:string;
    checked:boolean;
    children?:Checkbox[];
}
