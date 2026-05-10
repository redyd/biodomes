const T='#00d2b4',M='#7aada8',MU='#3d6b66',S='#132830',BR='rgba(0,210,180,0.15)';
const P=['#00d2b4','#f0a830','#5b8fa8','#a78bfa','#fb7185','#34d399','#f97316','#60a5fa'];
let D=[],C={},tab='species',days=30;

const $=id=>document.getElementById(id);
const el=(tag,attrs={},txt)=>{const e=document.createElement(tag);Object.assign(e,attrs);if(txt)e.textContent=txt;return e;};

function exportChart(cid,lid,name){
    const cv=$( cid);if(!cv)return;
    const items=[];
    if(lid){$( lid)?.querySelectorAll('.chart-legend-item').forEach(i=>{
        items.push({color:i.querySelector('.chart-legend-dot')?.style.background||'#fff',text:i.textContent.trim()});
    });}
    const PAD=20,DR=5,LH=24,COLS=3,FS=12;
    const lh=items.length?PAD+Math.ceil(items.length/COLS)*LH+PAD:0;
    const tc=el('canvas');tc.width=cv.width;tc.height=cv.height+lh;
    const ctx=tc.getContext('2d');
    ctx.fillStyle='#0d1f26';ctx.fillRect(0,0,tc.width,tc.height);
    ctx.drawImage(cv,0,0);
    if(items.length) {
        const cw=Math.floor(tc.width/COLS),sy=cv.height+PAD;
        ctx.font=`${FS}px monospace`;
        items.forEach(({color,text},i)=>{
            const x=PAD+(i%COLS)*cw,y=sy+Math.floor(i/COLS)*LH;
            ctx.fillStyle=color;ctx.beginPath();ctx.arc(x+DR,y+DR-1,DR,0,Math.PI*2);ctx.fill();
            ctx.fillStyle=M;ctx.fillText(text,x+DR*2+6,y+DR+4);
        });
    }
    const a=el('a',{download:`${name}-${new Date().toISOString().slice(0,10)}.png`,href:tc.toDataURL()});
    a.click();
}

function modal(title,lines){
    $('detail-title').textContent=title;
    $('detail-body').innerHTML=lines.map(l=>`<div class="detail-line"><span class="detail-key">${l.key}</span><span class="detail-val">${l.val}</span></div>`).join('');
    $('detail-modal').classList.add('open');
}

document.addEventListener('click',e=>{
    if(e.target.id==='detail-modal'||e.target.id==='detail-close')
        $('detail-modal').classList.remove('open');
});

const filtered=()=>{const v=$('biomeSelector').value;return v==='all'?D:D.filter(b=>b.slug===v);};

function legend(id,labels,colors){
    const el=$(id);if(!el)return;
    el.innerHTML=labels.map((l,i)=>`<span class="chart-legend-item"><span class="chart-legend-dot" style="background:${colors[i%colors.length]}"></span>${l}</span>`).join('');
}

const base=()=>({
    responsive:true,maintainAspectRatio:false,
    plugins:{legend:{display:false},tooltip:{backgroundColor:S,titleColor:T,bodyColor:M,borderColor:BR,borderWidth:1,titleFont:{family:'Space Mono'},bodyFont:{family:'Space Mono',size:11}}}
});

const ax=()=>({grid:{color:'rgba(0,210,180,0.06)'},ticks:{color:MU,font:{family:'Space Mono',size:10}},border:{color:'rgba(0,210,180,0.1)'}});

const lineOpts=(extra={})=>({
    responsive:true,maintainAspectRatio:false,
    plugins:{legend:{display:false},tooltip:{backgroundColor:S,titleColor:T,bodyColor:M,borderColor:BR,borderWidth:1,titleFont:{family:'Space Mono'},bodyFont:{family:'Space Mono',size:11}},...extra},
    scales:{x:{...ax(),title:{display:true,text:'Date',color:M}},y:{...ax(),beginAtZero:true,title:{display:true,text:'Nb espèces',color:M}}}
});

function renderSpecies(data){
    C.species?.destroy();
    const lb=data.map(b=>b.name),ct=data.map(b=>b.speciesCount);
    legend('leg-species',lb,P);
    const tot=ct.reduce((a,b)=>a+b,0);
    $('m-biomes').textContent=data.length;
    $('m-species').textContent=tot;
    $('m-avg').textContent=data.length?Math.round(tot/data.length):0;
    C.species=new Chart($('cSpecies'),{
        type:'bar',
        data:{labels:lb,datasets:[{data:ct,backgroundColor:lb.map((_,i)=>P[i%P.length]),borderRadius:4,borderSkipped:false}]},
        options:{...base(),
            onClick: (e, els) => {
                if (!els.length) return;
                const b = data[els[0].index];
                const lines = Object.entries(b.speciesByName ?? {})
                    .map(([name, count]) => ({ key: name, val: count + ' individu(s)' }));
                modal(b.name, lines.length ? lines : [{ key: 'Aucune espèce', val: '—' }]);
            },
            scales:{x:ax(),y:{...ax(),beginAtZero:true,ticks:{...ax().ticks,stepSize:1}}}}
    });
}

function renderConditions(data){
    ['cTemp','cHum'].forEach(id=>{C[id]?.destroy();});
    const lb=data.map(b=>b.name);
    const click=(e,els)=>{
        if(!els.length)return;
        const b=data[els[0].index];
        modal(b.name,[{key:'Température',val:b.temperature+' °C'},{key:'Humidité',val:b.humidity+' g/m³'}]);
    };
    C.cTemp=new Chart($('cTemp'),{type:'bar',
        data:{labels:lb,datasets:[{data:data.map(b=>b.temperature),backgroundColor:lb.map((_,i)=>i%2===0?'#fb7185':'#f97316'),borderRadius:4,borderSkipped:false}]},
        options:{...base(),onClick:click,scales:{x:ax(),y:{...ax(),ticks:{...ax().ticks,callback:v=>v+'°C'}}}}});
    C.cHum=new Chart($('cHum'),{type:'bar',
        data:{labels:lb,datasets:[{data:data.map(b=>b.humidity),backgroundColor:lb.map((_,i)=>i%2===0?T:'#60a5fa'),borderRadius:4,borderSkipped:false}]},
        options:{...base(),onClick:click,scales:{x:ax(),y:{...ax(),beginAtZero:true,ticks:{...ax().ticks,callback:v=>v+' g/m³'}}}}});
}

function renderTrophic(data){
    ['cDiet','cClass'].forEach(id=>{C[id]?.destroy();});
    const dt={},cl={};
    data.forEach(b=>{
        Object.entries(b.speciesByDiet||{}).forEach(([k,v])=>{dt[k]=(dt[k]||0)+v;});
        Object.entries(b.speciesByClassification||{}).forEach(([k,v])=>{cl[k]=(cl[k]||0)+v;});
    });
    const dL=Object.keys(dt),dV=Object.values(dt),cL=Object.keys(cl),cV=Object.values(cl);
    const dT=dV.reduce((a,b)=>a+b,0),cT=cV.reduce((a,b)=>a+b,0);
    legend('leg-diet',dL.map((l,i)=>`${l} ${Math.round(dV[i]/dT*100)}%`),P);
    legend('leg-class',cL.map((l,i)=>`${l} ${Math.round(cV[i]/cT*100)}%`),P);
    const donutClick=(labels,values,total)=>(e,els)=>{
        if(!els.length)return;
        const i=els[0].index;
        modal(labels[i],[{key:'Espèces',val:values[i]},{key:'Pourcentage',val:Math.round(values[i]/total*100)+'%'}]);
    };
    C.cDiet=new Chart($('cDiet'),{type:'doughnut',data:{labels:dL,datasets:[{data:dV,backgroundColor:P.slice(0,dL.length),borderWidth:0,hoverOffset:6}]},options:{...base(),cutout:'68%',onClick:donutClick(dL,dV,dT)}});
    C.cClass=new Chart($('cClass'),{type:'doughnut',data:{labels:cL,datasets:[{data:cV,backgroundColor:P.slice(0,cL.length),borderWidth:0,hoverOffset:6}]},options:{...base(),cutout:'68%',onClick:donutClick(cL,cV,cT)}});
}

async function loadHistory(){
    const v=$('biomeSelector').value,p=new URLSearchParams({days});
    if(v!=='all')p.append('biomeSlug',v);
    try{const r=await fetch(`?handler=HistoryData&${p}`);if(r.ok)renderHistory(await r.json());}
    catch(e){console.error(e);}
}

function histLine(canvasId,legendId,historyData,field){
    const dates=[...new Set(historyData.flatMap(b=>b[field].map(p=>p.date)))].sort();
    const labels=[...new Set(historyData.flatMap(b=>b[field].map(p=>p.label)))];
    legend(legendId,labels,P);
    C[canvasId]=new Chart($(canvasId),{type:'line',
        data:{labels:dates,datasets:labels.map((lbl,i)=>({
                label:lbl,
                data:dates.map(d=>historyData.reduce((s,b)=>{const p=b[field].find(p=>p.date===d&&p.label===lbl);return s+(p?p.count:0);},0)),
                borderColor:P[i%P.length],backgroundColor:P[i%P.length]+'33',fill:false,tension:0.4,pointRadius:4,pointHoverRadius:7
            }))},
        options:lineOpts()
    });
}

function renderHistory(h){
    ['cHistory','cDietHistory','cClassHistory'].forEach(id=>{C[id]?.destroy();});
    $('history-empty').style.display=h.length?'none':'block';
    if(!h.length)return;
    const dates=[...new Set(h.flatMap(b=>b.speciesOverTime.map(p=>p.date)))].sort();
    legend('leg-history',h.map(b=>b.biomeName),h.map((_,i)=>P[i%P.length]));
    C.cHistory=new Chart($('cHistory'),{type:'line',
        data:{labels:dates,datasets:h.map((b,i)=>({
                label:b.biomeName,
                data:dates.map(d=>{const p=b.speciesOverTime.find(p=>p.date===d);return p?p.count:null;}),
                borderColor:P[i%P.length],backgroundColor:P[i%P.length]+'22',fill:true,tension:0.4,pointRadius:4,pointHoverRadius:7,spanGaps:true
            }))},
        options:lineOpts({tooltip:{backgroundColor:S,titleColor:T,bodyColor:M,borderColor:BR,borderWidth:1,titleFont:{family:'Space Mono'},bodyFont:{family:'Space Mono',size:11},callbacks:{label:c=>`${c.dataset.label} : ${c.parsed.y} espèce(s)`}}})
    });
    histLine('cDietHistory','leg-diet-history',h,'byDietOverTime');
    histLine('cClassHistory','leg-class-history',h,'byClassificationOverTime');
}

function renderAll(){const d=filtered();renderSpecies(d);renderConditions(d);renderTrophic(d);}

document.querySelectorAll('.stats-tab').forEach(btn=>btn.addEventListener('click',()=>{
    document.querySelectorAll('.stats-tab').forEach(b=>b.classList.remove('active'));
    document.querySelectorAll('.stats-pane').forEach(p=>p.classList.remove('active'));
    btn.classList.add('active');
    tab=btn.dataset.pane;
    $('pane-'+tab).classList.add('active');
    if(tab==='history')loadHistory();
}));

$('biomeSelector').addEventListener('change',()=>{renderAll();if(tab==='history')loadHistory();});

document.querySelectorAll('.period-btn').forEach(btn=>btn.addEventListener('click',()=>{
    document.querySelectorAll('.period-btn').forEach(b=>b.classList.remove('active'));
    btn.classList.add('active');
    days=parseInt(btn.dataset.days);
    loadHistory();
}));

async function loadData(){
    try{
        const r=await fetch('?handler=StatsData');if(!r.ok)return;
        D=await r.json();
        const sel=$('biomeSelector');
        sel.innerHTML='<option value="all">Tous mes biomes</option>';
        D.forEach(b=>{const o=el('option',{value:b.slug},b.name);sel.appendChild(o);});
        renderAll();
    }catch(e){console.error(e);}
}

loadData();