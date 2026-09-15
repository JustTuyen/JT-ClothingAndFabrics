//
import './Css/Suggestion.css'
//
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
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
//
import ElectricBoltOutlinedIcon from '@mui/icons-material/ElectricBoltOutlined';
import RemoveRedEyeOutlinedIcon from '@mui/icons-material/RemoveRedEyeOutlined';
import EmojiObjectsIcon from '@mui/icons-material/EmojiObjects';
//

export default function SuggestBox(){
    return(
        <>
        <div className="w-full p-4 flex flex-col gap-4">
            <div className="p-2 flex gap-2 items-center">
                <EmojiObjectsIcon/>
                <h1 className="text-xl 
                font-bold 
                text-black leading-tight tracking-tight">
                    Sản Phẩm Liên Quan
                </h1>
            </div>
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 p-2 gap-4">
                <Card sx={{ maxWidth: 345}}>
                    <CardActionArea>
                        <div className="relative flex">
                            <div className="absolute top-0 right-0 p-3 z-10">
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
                            <Tooltip title="Xem trước">
                                <IconButton sx={{ backgroundColor: '#BFC9D1' }} size="medium">
                                    <RemoveRedEyeOutlinedIcon fontSize="inherit" />
                                </IconButton>
                            </Tooltip>
                        </CardActions>  
                    </CardContent>
                </Card>              
            </div>
        </div>
        </>
    )
}