import Navbar from "../../components/NavBar"
import Banner from '../../components/Banner'
import Testing from '../../components/Discount'
import Banner01 from '../../assets/banner01.webp'
import Banner02 from '../../assets/banner02.webp'
const IMAGES = [
  { url: Banner01, alt: "Car One" },
  { url: Banner02, alt: "Car Two" },
]

export default function Home(){
    
    return(
        <>
        <Navbar/>
        <div className="min-h-screen flex flex-col gap-4">
            <Banner images={IMAGES} />
            <div className="bg-amber-100 flex justify-center">
                <Testing/>
            </div>
        </div>

        </>
    )
}