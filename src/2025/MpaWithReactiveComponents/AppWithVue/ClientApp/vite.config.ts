import {defineConfig} from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
    plugins: [vue()],
    server: {
        port: 5173,
        strictPort: true,
        https: false,
        hmr: { protocol: 'ws', host: 'localhost', port: 5173 },
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
