import { defineConfig } from 'vite';

export default defineConfig({
  optimizeDeps: {
    exclude: ['@angular/ssr'] // или точния идентификатор, който се оплаква (например, '@angular/ssr' или '@angular_ssr')
  }
});