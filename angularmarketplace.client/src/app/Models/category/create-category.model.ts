import { Category } from "./category.model";

export interface CreateCategory {

    isSubCategory:boolean;
    title:string;
    readonly mask:string;
    url_Title:string;
    img?:File;
    parent?:Category;
    svg?:string;
}
