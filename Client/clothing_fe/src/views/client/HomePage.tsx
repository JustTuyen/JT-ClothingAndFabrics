import Navbar from "../../components/NavBar"
import Banner from '../../components/Home/Banner'
import Discount from '../../components/Home/Discount'
import DisplayTab from "../../components/Home/DisplayTab"
import CategoryLog from "../../components/Home/CategoryLog"
import Footer from "../../components/Footer"
import DetailCard from "../../components/ProductDetailCard"
import { formatPrice } from "../../store/Ult"
//
import Banner01 from '../../assets/banner01.webp'
import Banner02 from '../../assets/banner02.webp'
import Place from '../../assets/place.webp'
//
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Button from '@mui/material/Button';
import CardActionArea from '@mui/material/CardActionArea';
import CardActions from '@mui/material/CardActions';
import Typography from '@mui/material/Typography';
import Tooltip from '@mui/material/Tooltip';
//icons
import LocalFireDepartmentIcon from '@mui/icons-material/LocalFireDepartment';
import ElectricBoltOutlinedIcon from '@mui/icons-material/ElectricBoltOutlined';
import WhatshotOutlinedIcon from '@mui/icons-material/WhatshotOutlined';

//css
import './css/Home.css'

// const IMAGES = [
//   { url: Banner01, alt: "Car One" },
//   { url: Banner02, alt: "Car Two" },
// ]

export default function Home(){
    
    return(
        <>
        <Navbar/>
        <div className="min-h-screen flex flex-col gap-8">
            <Banner  />
            <div className="flex justify-center">
                <Discount/>
            </div>
            <div className="p-4 bg-[#FFF1D1]">
                <div className="p-2 flex gap-2 text-[24px] items-center">
                    <LocalFireDepartmentIcon id='fire-icon' sx={{fontSize: 'inherit', color: '#DF301C'}}/>
                    <p className="font-bold">Top <span className="text-[32px] underline">10</span> Sản Phẩm của KingDom</p>
                </div>
                <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 p-2 gap-4">
                    <Card sx={{ maxWidth: 345}}>
                        <CardActionArea>
                            <div className="relative flex">
                                <div className="absolute top-0 left-0 p-3 z-10">
                                    <Stack direction="row" spacing={1}>
                                        <Chip icon={<WhatshotOutlinedIcon color="inherit" sx={{ color: 'white'}}/>} 
                                        label="Sản phẩm nổi bật"
                                        sx={{backgroundColor: '#FF9100', color: 'white', fontWeight: 'bold'}}/>
                                    </Stack>
                                </div>
                                <div className="absolute top-10 md:top-0 left-0 md:left-auto md:right-0 p-3 z-10">
                                    <Stack direction="row" spacing={1}>
                                        <Tooltip title="Giảm giá">
                                            <Chip icon={<ElectricBoltOutlinedIcon color="inherit" sx={{ color: 'white'}}/>} 
                                            label="-14%" id='discount-chip'
                                            sx={{backgroundColor: '#DF301C', color: 'white', fontWeight: 'bold'}}/>
                                        </Tooltip>
                                    </Stack>
                                </div>
                                <CardMedia
                                component="img"
                                height="140"
                                image={Place}
                                alt="green iguana"
                                />
                            </div>
                        </CardActionArea>                        
                        <CardContent>
                            <div className="flex flex-col gap-2">
                                <Typography variant="body2" sx={{ color: 'black'}}>
                                    Áo Sơ Mi Tay Ngắn Slippery - 88635 - Big Size Upto 5XL
                                </Typography>
                                <Typography variant="caption" sx={{ color: '#DF301C', fontWeight: '800' }}>
                                    310,000₫ 
                                    <span className="text-[#333] font-semibold line-through mx-2">
                                        310,000₫
                                    </span>
                                </Typography>
                            </div>
                            <CardActions sx={{justifyContent: 'center', gap: '4px'}} >
                                <Button variant="contained" size="large"
                                sx={{ backgroundColor: '#DF301C', borderRadius: '16px'}}>
                                    Thêm vào giỏ
                                </Button>
                                <DetailCard/>
                            </CardActions>  
                        </CardContent>
                    </Card>              
                </div>
            </div>
            <div className="">
                <DisplayTab/>
            </div>
            <div className="">
                <CategoryLog/>
            </div>
        </div>
        <Footer/>
        </>
    )
}