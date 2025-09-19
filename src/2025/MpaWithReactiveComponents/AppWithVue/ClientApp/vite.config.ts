import {defineConfig} from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
    plugins: [vue()],
    server: {
        port: 5174,
        strictPort: true,
        https: false,
        hmr: { protocol: 'ws' }, 
        cors: true
    },
    build: {
        outDir: '../wwwroot/app',
        emptyOutDir: true,
        manifest: true,
        rollupOptions: {
            input: '/src/main.tsx',
        },
    }
})
