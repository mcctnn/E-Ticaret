import { nanoid } from 'nanoid';

export type CartType = {
    id: string;
    items: CartItem[];
    deliveryMethodId?: string;
    paymentIntentId?: string;
    clientSecret?: string;
};

export type CartItem = {
    productId: string;
    productName: string;
    price: number;
    quantity: number;
    brand: string;
    type: string;
    pictureUrl: string;
}

export class Cart implements CartType {
    id = nanoid();
    items: CartItem[] = [];
    deliveryMethodId?: string;
    paymentIntentId?: string;
    clientSecret?: string;
}