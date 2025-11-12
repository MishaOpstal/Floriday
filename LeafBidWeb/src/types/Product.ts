export interface Product {
    productId: number;
    productName: string;
    productQuantity: number;
    productPrice?: number;
    productImage?: string;
    productDescription?: string;
}