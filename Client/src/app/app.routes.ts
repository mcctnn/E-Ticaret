import { Routes } from '@angular/router';
import { Home } from './features/home/home';
import { Shop } from './features/shop/shop';
import { ProductDetails } from './features/shop/product-details/product-details';
import { TestError } from './features/test-error/test-error';
import { ServerError } from './shared/components/server-error/server-error';
import { NotFound } from './shared/components/not-found/not-found';
import { Cart } from './features/cart/cart';
import { authGuard } from './core/guards/auth-guard';
import { adminGuard } from './core/guards/admin-guard';



export const routes: Routes = [
    { path: '', component: Home },
    { path: 'shop', component: Shop },
    { path: 'shop/:id', component: ProductDetails },
    { path: 'cart', component: Cart },
    { path: 'checkout', loadChildren: () => import('./features/checkout/checkout-routes').then(m => m.checkoutRoutes) },
    { path: 'orders', loadChildren: () => import('./features/orders/order-routes').then(m => m.orderRoutes) },
    { path: 'account', loadChildren: () => import('./features/account/account-routes').then(m => m.accountRoutes) },
    { path: 'test-error', component: TestError },
    { path: 'not-found', component: NotFound },
    { path: 'server-error', component: ServerError },
    { path: 'admin', loadComponent: () => import('./features/admin/admin').then(m => m.Admin), canActivate: [authGuard, adminGuard] },
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];
