import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { createBrowserRouter, RouterProvider } from 'react-router'
import Home from './views/client/HomePage'
import NotFound from './views/client/NotFoundPage'

const router = createBrowserRouter([
  {path:'/', element:<Home/>},
  {path:'*', element:<NotFound/>},
])
createRoot(document.getElementById('root')!).render(
  <StrictMode>
      <RouterProvider router={router}/>
  </StrictMode>,
)
