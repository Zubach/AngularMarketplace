export interface CreateProduct {
    title:string;
    description:string;
    price:string;
    categoryMask:string;
    imgs?:FileList;
    producerId:number;
}
