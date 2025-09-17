import {defineConfig} from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [react()],
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
